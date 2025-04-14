#ifndef GET_LOD_FADE_INCLUDED
#define GET_LOD_FADE_INCLUDED

inline void GetLODFade_float(out float LODFade) {
	LODFade = 1;

	if (unity_LODFade.x > 0) {
		LODFade = unity_LODFade.x;
	}
}

#endif