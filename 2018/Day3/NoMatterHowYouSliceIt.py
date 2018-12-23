import re


class Patch:

    def __init__(self, string_patch):
        patch_regex = re.match(r"#(?P<id>\d+)\s@\s(?P<x>\d+),(?P<y>\d+):\s(?P<width>\d+)x(?P<height>\d+)", string_patch)
        self.id = int(patch_regex.group('id'))
        self.x = int(patch_regex.group('x'))
        self.y = int(patch_regex.group('y'))
        self.width = int(patch_regex.group('width'))
        self.height = int(patch_regex.group('height'))

    def to_tuples(self):
        for x in range(self.x, self.x + self.width):
            for y in range(self.y, self.y + self.height):
                yield x, y


if __name__ == '__main__':
    patches = list(map(lambda string_patch: Patch(string_patch), open("input.txt", "r").readlines()))

    used_patches = set()
    double_used_patches = set()
    for patch in patches:
        inches = patch.to_tuples()
        for inch in inches:
            if inch in used_patches:
                double_used_patches.add(inch)
            else:
                used_patches.add(inch)

    print(len(double_used_patches))

    for patch in patches:
        inches = patch.to_tuples()
        overlapping = False
        for inch in inches:
            if inch in double_used_patches:
                overlapping = True

        if not overlapping:
            print(patch.id)
