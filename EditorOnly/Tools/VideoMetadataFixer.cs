using UnityEngine;
using UnityEditor;
using UnityEngine.Video;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

public class VideoMetadataFixer : EditorWindow
{
    private VideoClip selectedVideo;
    private bool isFfmpegAvailable = false;
    private bool isChecking = false;
    private bool isProcessing = false;
    private string statusMessage = "";

    [MenuItem("Dev'loader/Tools/Fix video metadatas")]
    public static void ShowWindow()
    {
        GetWindow<VideoMetadataFixer>("Fix Video Metadatas");
    }

    private void OnEnable()
    {
        CheckFfmpegAvailability();
    }

    private void OnGUI()
    {
        GUILayout.Label("Fix Video Color Primaries", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        if (isChecking)
        {
            EditorGUILayout.HelpBox("Vérification de ffmpeg...", MessageType.Info);
            return;
        }

        if (!isFfmpegAvailable)
        {
            EditorGUILayout.HelpBox(
                "ffmpeg n'est pas détecté dans le PATH système.\n" +
                "Veuillez télécharger et installer ffmpeg pour utiliser cet outil.",
                MessageType.Warning
            );

            if (GUILayout.Button("Télécharger ffmpeg"))
            {
                Application.OpenURL("https://ffmpeg.org/download.html");
            }

            EditorGUILayout.Space();

            if (GUILayout.Button("Revérifier ffmpeg"))
            {
                CheckFfmpegAvailability();
            }

            return;
        }

        EditorGUILayout.HelpBox("ffmpeg détecté ✓", MessageType.Info);
        EditorGUILayout.Space();

        EditorGUI.BeginDisabledGroup(isProcessing);
        selectedVideo = (VideoClip)EditorGUILayout.ObjectField(
            "VideoClip",
            selectedVideo,
            typeof(VideoClip),
            false
        );
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.Space();

        EditorGUI.BeginDisabledGroup(selectedVideo == null || isProcessing);
        if (GUILayout.Button(isProcessing ? "Traitement en cours..." : "Fix", GUILayout.Height(30)))
        {
            FixVideoMetadata();
        }
        EditorGUI.EndDisabledGroup();

        if (!string.IsNullOrEmpty(statusMessage))
        {
            EditorGUILayout.Space();
            EditorGUILayout.HelpBox(statusMessage, MessageType.Info);
        }
    }

    private void CheckFfmpegAvailability()
    {
        isChecking = true;
        statusMessage = "";
        Repaint();

        try
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = "-version",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            using (Process process = Process.Start(startInfo))
            {
                process.WaitForExit();
                isFfmpegAvailable = process.ExitCode == 0;
            }
        }
        catch
        {
            isFfmpegAvailable = false;
        }

        isChecking = false;
        Repaint();
    }

    private void FixVideoMetadata()
    {
        isProcessing = true;
        statusMessage = "Récupération des métadonnées...";
        Repaint();

        string videoPath = AssetDatabase.GetAssetPath(selectedVideo);
        string fullPath = Path.GetFullPath(videoPath);
        string directory = Path.GetDirectoryName(fullPath);
        string outputPath = Path.Combine(directory, "output.mp4");
        string backupPath = fullPath + ".backup";

        try
        {
            // Étape 1: Récupérer les métadonnées
            string colorPrimaries = GetColorPrimaries(fullPath);

            if (string.IsNullOrEmpty(colorPrimaries))
            {
                colorPrimaries = "1"; // Valeur par défaut (bt709)
                statusMessage = "Métadonnée Color primaries non trouvée, utilisation de la valeur par défaut (bt709)";
                Repaint();
            }

            // Étape 2: Exécuter ffmpeg
            statusMessage = "Conversion de la vidéo...";
            Repaint();

            string arguments = $"-i \"{fullPath}\" -color_primaries {colorPrimaries} -color_trc {colorPrimaries} -colorspace {colorPrimaries} -color_range pc -vcodec libx264 -profile:v baseline \"{outputPath}\"";

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            using (Process process = Process.Start(startInfo))
            {
                process.WaitForExit();

                if (process.ExitCode != 0)
                {
                    string error = process.StandardError.ReadToEnd();
                    throw new System.Exception($"Erreur ffmpeg: {error}");
                }
            }

            // Étape 3: Créer le backup
            statusMessage = "Création du backup...";
            Repaint();

            if (File.Exists(backupPath))
            {
                File.Delete(backupPath);
            }
            File.Copy(fullPath, backupPath);

            // Étape 4: Remplacer le fichier original
            statusMessage = "Remplacement du fichier...";
            Repaint();

            File.Delete(fullPath);
            File.Move(outputPath, fullPath);

            // Rafraîchir Unity
            AssetDatabase.Refresh();

            statusMessage = "✓ Vidéo corrigée avec succès ! Un backup a été créé.";
            UnityEngine.Debug.Log($"Vidéo corrigée: {fullPath}");
        }
        catch (System.Exception e)
        {
            statusMessage = $"Erreur: {e.Message}";
            UnityEngine.Debug.LogError($"Erreur lors de la correction de la vidéo: {e.Message}");

            // Nettoyer les fichiers temporaires
            if (File.Exists(outputPath))
            {
                File.Delete(outputPath);
            }
        }
        finally
        {
            isProcessing = false;
            Repaint();
        }
    }

    private string GetColorPrimaries(string videoPath)
    {
        try
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = $"-i \"{videoPath}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            using (Process process = Process.Start(startInfo))
            {
                string output = process.StandardError.ReadToEnd();
                process.WaitForExit();

                // Chercher "color_primaries" dans la sortie
                Match match = Regex.Match(output, @"color_primaries[:\s]+(\d+)", RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    return match.Groups[1].Value;
                }
            }
        }
        catch (System.Exception e)
        {
            UnityEngine.Debug.LogWarning($"Impossible de récupérer color_primaries: {e.Message}");
        }

        return null;
    }
}