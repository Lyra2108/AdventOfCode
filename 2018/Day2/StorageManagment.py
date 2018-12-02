from collections import defaultdict
from itertools import groupby
from pprint import pprint

if __name__ == "__main__":
    box_ids = open("boxids.txt", "r").readlines()
    char_counts = list(map(lambda box_id: [len(
        list(group)) for k, group in groupby(sorted(box_id))], box_ids))
    two_times = 0
    three_times = 0
    for char_count in char_counts:
        if 2 in char_count:
            two_times += 1
        if 3 in char_count:
            three_times += 1

    print(two_times * three_times)

    word_groups = defaultdict(set)
    for box_id in box_ids:
        box_id_list = list(box_id.strip())
        for i in range(len(box_id_list)):
            copyed_id = box_id_list.copy()
            del copyed_id[i]
            word_groups[''.join(copyed_id)].add(box_id)

    result = {k: v for k, v in word_groups.items() if len(v) > 1}

    pprint(result)
