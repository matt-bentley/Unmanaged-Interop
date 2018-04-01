#pragma once
#include <string>

#define INTEROP_STRING const char16_t*
#define INTEROP_BOOL char
#define INTEROP_BOOL_TRUE ((INTEROP_BOOL) 1)
#define INTEROP_BOOL_FALSE ((INTEROP_BOOL) 0)

extern "C"
{
	__declspec(dllexport) int __stdcall levenshtein_distance(INTEROP_STRING str1, int len1, INTEROP_STRING str2, int len2);
	__declspec(dllexport) int __stdcall levenshtein_distance_max(INTEROP_STRING str1, int len1, INTEROP_STRING str2, int len2, int max_distance);
	__declspec(dllexport) INTEROP_BOOL __stdcall is_levenshtein_distance_max(INTEROP_STRING str1, int len1, INTEROP_STRING str2, int len2, int max_distance);
}

