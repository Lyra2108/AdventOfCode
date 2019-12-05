from enum import IntEnum


class Operation(IntEnum):
    Add = 1,
    Multiply = 2,
    Input = 3,
    Output = 4,
    JumpTrue = 5,
    JumpFalse = 6.
    LessThen = 7,
    Equals = 8,
    Term = 99


class Opcode(object):
    def __init__(self, opcode_int):
        opcode_str = '{:05d}'.format(opcode_int)
        self.operation = int(opcode_str[3:])
        self.immediate_param1 = bool(int(opcode_str[2]))
        self.immediate_param2 = bool(int(opcode_str[1]))
        self.immediate_param3 = bool(int(opcode_str[0]))

    def __len__(self):
        if self.operation in (Operation.Add, Operation.Multiply, Operation.LessThen, Operation.Equals):
            return 4
        if self.operation in (Operation.JumpFalse, Operation.JumpTrue):
            return 3
        if self.operation in (Operation.Input, Operation.Output):
            return 2
        if self.operation == Operation.Term:
            return 1

    def execute(self, int_pointer, ints, input):
        param1 = ints[int_pointer + 1]
        first = param1 if self.immediate_param1 else ints[param1]

        if self.operation == Operation.Output:
            print(first)
            return len(self)

        if self.operation == Operation.Input:
            ints[param1] = input
            return len(self)

        param2 = ints[int_pointer + 2]
        second = param2 if self.immediate_param2 else ints[param2]

        if self.operation == Operation.JumpTrue:
            if first:
                return second - int_pointer
            return len(self)

        if self.operation == Operation.JumpFalse:
            if not first:
                return second - int_pointer
            return len(self)

        target = ints[int_pointer + 3]
        if self.operation == Operation.Add:
            ints[target] = first + second
        elif self.operation == Operation.Multiply:
            ints[target] = first * second
        elif self.operation == Operation.LessThen:
            ints[target] = int(first < second)
        elif self.operation == Operation.Equals:
            ints[target] = int(first == second)

        return len(self)


def intcode(input_str, noun=None, verb=None, input=None):
    ints = list(map(lambda x: int(x), input_str.split(",")))

    if noun:
        ints[1] = noun
    if verb:
        ints[2] = verb

    int_pointer = 0
    while int_pointer < len(ints):
        opcode = Opcode(ints[int_pointer])
        if opcode.operation == Operation.Term:
            return ints[0]

        int_pointer += opcode.execute(int_pointer, ints, input)


def find_noun_and_verb(input):
    for verb in range(0, 100):
        for noun in range(0, 100):
            if intcode(input, noun, verb) == 19690720:
                return noun, verb


if __name__ == '__main__':
    input_day2 = open('input_day2.txt').readline()
    assert 3500 == intcode("1,9,10,3,2,3,11,0,99,30,40,50")
    assert 3895705 == intcode(input_day2, 12, 2)
    assert (64, 17) == find_noun_and_verb(input_day2)

    intcode(open('input.txt').readline(), input=1)
    print("---------------------")
    intcode(open('input.txt').readline(), input=5)
