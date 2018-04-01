#include "stdafx.h"
#include "CppInterop.h"
#include <numeric>
#include <algorithm>

int levenshtein_distance(INTEROP_STRING s1, int len1, INTEROP_STRING s2, int len2) {

	auto column_start = (decltype(len1))1;
	auto column = new decltype(len1)[len1 + 1];

	std::iota(column + column_start - 1, column + len1 + 1, column_start - 1);

	for (auto x = column_start; x <= len2; x++) {
		column[0] = x;
		auto last_diagonal = x - column_start;
		for (auto y = column_start; y <= len1; y++) {
			auto old_diagonal = column[y];
			auto possibilities = {
				column[y] + 1,
				column[y - 1] + 1,
				last_diagonal + (s1[y - 1] == s2[x - 1] ? 0 : 1)
			};
			column[y] = std::min(possibilities);
			last_diagonal = old_diagonal;
		}
	}
	auto result = column[len1];
	delete[] column;
	return result;
}

int levenshtein_distance_max(INTEROP_STRING s1, int len1, INTEROP_STRING s2, int len2, int max_distance) {
	auto column_start = (decltype(len1))1;
	auto column = new decltype(len1)[len1 + 1];

	std::iota(column + column_start - 1, column + len1 + 1, column_start - 1);

	for (auto x = column_start; x <= len2; x++) {
		if (column[x - 1] > max_distance) {
			return max_distance;
		}
		column[0] = x;
		auto last_diagonal = x - column_start;
		for (auto y = column_start; y <= len1; y++) {
			auto old_diagonal = column[y];
			auto possibilities = {
				column[y] + 1,
				column[y - 1] + 1,
				last_diagonal + (s1[y - 1] == s2[x - 1] ? 0 : 1)
			};
			column[y] = std::min(possibilities);
			last_diagonal = old_diagonal;
		}
	}
	auto result = column[len1];
	delete[] column;
	return result;
}

INTEROP_BOOL is_levenshtein_distance_max(INTEROP_STRING s1, int len1, INTEROP_STRING s2, int len2, int max_distance) {
	auto column_start = (decltype(len1))1;
	auto column = new decltype(len1)[len1 + 1];

	std::iota(column + column_start - 1, column + len1 + 1, column_start - 1);

	for (auto x = column_start; x <= len2; x++) {
		if (column[x - 1] > max_distance) {
			return INTEROP_BOOL_TRUE;
		}
		column[0] = x;
		auto last_diagonal = x - column_start;
		for (auto y = column_start; y <= len1; y++) {
			auto old_diagonal = column[y];
			auto possibilities = {
				column[y] + 1,
				column[y - 1] + 1,
				last_diagonal + (s1[y - 1] == s2[x - 1] ? 0 : 1)
			};
			column[y] = std::min(possibilities);
			last_diagonal = old_diagonal;
		}
	}
	auto result = column[len1];
	delete[] column;
	if (result > max_distance) {
		return INTEROP_BOOL_TRUE;
	}
	else {
		return INTEROP_BOOL_FALSE;
	}
}