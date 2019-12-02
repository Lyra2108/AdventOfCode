def intcode(input_str, noun=None, verb=None):
    ints = list(map(lambda x: int(x), input_str.split(",")))

    if noun:
        ints[1] = noun
    if verb:
        ints[2] = verb

    for int_pointer in range(0, len(ints), 4):
        opcode = ints[int_pointer]
        if opcode == 99:
            return ints[0]

        param1 = ints[int_pointer + 1]
        param2 = ints[int_pointer + 2]
        target = ints[int_pointer + 3]

        first = ints[param1]
        second = ints[param2]

        if opcode == 1:
            ints[target] = first + second
        elif opcode == 2:
            ints[target] = first * second


def find_noun_and_verb():
    for verb in range(0, 100):
        for noun in range(0, 100):
            if intcode(input, noun, verb) == 19690720:
                return noun, verb


if __name__ == '__main__':
    input = open('input.txt').readline()

    assert 3500 == intcode("1,9,10,3,2,3,11,0,99,30,40,50")

    print("The program was at state %d" % intcode(input, 12, 2))
    print("The noun %d and the verb %d" % find_noun_and_verb())
