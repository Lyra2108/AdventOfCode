from concurrent.futures.thread import ThreadPoolExecutor
from enum import IntEnum
from queue import Queue

from defaultlist import defaultlist


class Operation(IntEnum):
    Add = 1,
    Multiply = 2,
    Input = 3,
    Output = 4,
    JumpTrue = 5,
    JumpFalse = 6.
    LessThen = 7,
    Equals = 8,
    SetRelativeBase = 9,
    Term = 99


class Mode(IntEnum):
    Position = 0,
    Absolute = 1,
    Relative = 2


class Opcode(object):
    def __init__(self, opcode_int):
        opcode_str = '{:05d}'.format(opcode_int)
        self.operation = Operation(int(opcode_str[3:]))
        self.mode_param1 = Mode(int(opcode_str[2]))
        self.mode_param2 = Mode(int(opcode_str[1]))
        self.mode_param3 = Mode(int(opcode_str[0]))

    def __len__(self):
        if self.operation in (Operation.Add, Operation.Multiply, Operation.LessThen, Operation.Equals):
            return 4
        if self.operation in (Operation.JumpFalse, Operation.JumpTrue):
            return 3
        if self.operation in (Operation.Input, Operation.Output, Operation.SetRelativeBase):
            return 2
        if self.operation == Operation.Term:
            return 1

    def execute(self, int_pointer, ints, input, output, relative_base):
        first, param1 = self.resolve_param(int_pointer + 1, self.mode_param1, ints, relative_base)
        if self.operation == Operation.Output:
            output.put(first)
            return int_pointer + len(self), relative_base
        if self.operation == Operation.Input:
            ints[param1] = input.get()
            return int_pointer + len(self), relative_base
        if self.operation == Operation.SetRelativeBase:
            return int_pointer + len(self), first + relative_base

        second, param2 = self.resolve_param(int_pointer + 2, self.mode_param2, ints, relative_base)
        if self.operation == Operation.JumpTrue:
            if first:
                return second, relative_base
            return int_pointer + len(self), relative_base
        if self.operation == Operation.JumpFalse:
            if not first:
                return second, relative_base
            return int_pointer + len(self), relative_base

        _, target = self.resolve_param(int_pointer + 3, self.mode_param3, ints, relative_base)
        if self.operation == Operation.Add:
            ints[target] = first + second
        elif self.operation == Operation.Multiply:
            ints[target] = first * second
        elif self.operation == Operation.LessThen:
            ints[target] = int(first < second)
        elif self.operation == Operation.Equals:
            ints[target] = int(first == second)

        return int_pointer + len(self), relative_base

    def resolve_param(self, param_pointer, param_mode, ints, relative_base):
        param = ints[param_pointer]
        if param_mode == Mode.Absolute:
            return param, None
        elif param_mode == Mode.Position:
            return ints[param], param
        elif param_mode == Mode.Relative:
            return ints[param + relative_base], param + relative_base


def intcode(input_str, input=None, output=None):
    ints = defaultlist(lambda: 0)
    for _int in map(lambda x: int(x), input_str.split(",")):
        ints.append(_int)

    if not input:
        input = Queue()

    if not output:
        output = input

    int_pointer = 0
    relative_base = 0
    while int_pointer < len(ints):
        opcode = Opcode(ints[int_pointer])
        if opcode.operation == Operation.Term:
            return list(output.queue)[-1] if output.queue else ints[0]

        int_pointer, relative_base = opcode.execute(int_pointer, ints, input, output, relative_base)


def max_thrust(input):
    thrusts = []

    start = 0
    stop = 5
    for a in range(start, stop):
        input_a = setup_queue(a, 0)
        thrust_a = intcode(input, input=input_a)
        for b in set(range(start, stop)) - {a}:
            thrust_b = intcode(input, input=setup_queue(b, thrust_a))
            for c in set(range(start, stop)) - {a, b}:
                thrust_c = intcode(input, input=setup_queue(c, thrust_b))
                for d in set(range(start, stop)) - {a, b, c}:
                    e = (set(range(start, stop)) - {a, b, c, d}).pop()

                    thrust_d = intcode(input, input=setup_queue(d, thrust_c))
                    thrust_e = intcode(input, input=setup_queue(e, thrust_d))
                    thrusts.append(thrust_e)
    return max(thrusts)


def max_thrust_looped(input):
    thrusts = []

    start = 5
    stop = 10
    for a in range(start, stop):
        for b in set(range(start, stop)) - {a}:
            for c in set(range(start, stop)) - {a, b}:
                for d in set(range(start, stop)) - {a, b, c}:
                    e = (set(range(start, stop)) - {a, b, c, d}).pop()

                    output_a = setup_queue(b)
                    output_b = setup_queue(c)
                    output_c = setup_queue(d)
                    output_d = setup_queue(e)
                    output_e = setup_queue(a, 0)

                    with ThreadPoolExecutor() as executor:
                        executor.submit(intcode, input, output_e, output_a)
                        executor.submit(intcode, input, output_a, output_b)
                        executor.submit(intcode, input, output_b, output_c)
                        executor.submit(intcode, input, output_c, output_d)
                        final = executor.submit(intcode, input, output_d, output_e)

                        result = final.result()
                        thrusts.append(result)
    return max(thrusts)


def setup_queue(*values):
    queue = Queue()
    for value in values:
        queue.put(value)
    return queue


if __name__ == '__main__':
    assert 43210 == max_thrust("3,15,3,16,1002,16,10,16,1,16,15,15,4,15,99,0,0")
    assert 54321 == max_thrust("3,23,3,24,1002,24,10,24,1002,23,-1,23,101,5,23,23,1,24,23,23,4,23,99,0,0")
    assert 65210 == max_thrust(
        "3,31,3,32,1002,32,10,32,1001,31,-2,31,1007,31,0,33,1002,33,7,33,1,33,31,31,1,32,31,31,4,31,99,0,0,0")

    assert 139629729 == max_thrust_looped(
        "3,26,1001,26,-4,26,3,27,1002,27,2,27,1,27,26,27,4,27,1001,28,-1,28,1005,28,6,99,0,0,5")

    assert 1125899906842624 == intcode("104,1125899906842624,99")
    assert 16 == len('{}'.format(intcode("1102,34915192,34915192,7,4,7,99,0")))
    assert 99 == intcode("109,1,204,-1,1001,100,1,100,1008,100,16,101,1006,101,0,99")

    input = open('input.txt').readline()
    print(intcode(input, setup_queue(1)))
    print(intcode(input, setup_queue(2)))
