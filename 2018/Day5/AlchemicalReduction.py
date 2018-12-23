def reduce(polymer):
    while True:
        changed = False
        previous_char = None
        copy = polymer.copy()

        for i in range(len(copy)-1, -1, -1):
            char = copy[i]
            if not previous_char:
                previous_char = char
                continue

            if char != previous_char and (char == previous_char.lower() or char.lower() == previous_char):
                del polymer[i+1]
                del polymer[i]
                changed = True
                previous_char = None
            else:
                previous_char = char

        if not changed:
            break

    return len(polymer)


def char_range(c1, c2):
    """Generates the characters from `c1` to `c2`, inclusive."""
    for c in range(ord(c1), ord(c2)+1):
        yield chr(c)


if __name__ == '__main__':
    polymer = open('input.txt', 'r').readline().strip()

    print(reduce(list(polymer)))

    lengths = set()
    for c in char_range('a', 'z'):
        char_polymer = polymer.replace(c, '').replace(c.upper(), '')
        lengths.add(reduce(list(char_polymer)))

    print(min(lengths))
