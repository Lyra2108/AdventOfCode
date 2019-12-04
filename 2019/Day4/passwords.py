from collections import Counter


def passwords(start, end):
    guesses = []
    for number in range(start, end + 1):
        digits = list(map(lambda char: int(char), f"{number}"))
        if not (digits[0] <= digits[1] <= digits[2] <= digits[3] <= digits[4] <= digits[5]):
            continue

        counts = Counter(digits)
        if 2 in counts.values():
            guesses.append(number)

    return guesses


if __name__ == '__main__':
    print(passwords(111111, 111123))
    print(len(passwords(402328, 864247)))
