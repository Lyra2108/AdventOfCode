from concurrent.futures.thread import ThreadPoolExecutor
from enum import IntEnum
from queue import Queue


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

    def execute(self, int_pointer, ints, input, output):
        param1 = ints[int_pointer + 1]
        first = param1 if self.immediate_param1 else ints[param1]

        if self.operation == Operation.Output:
            output.put(first)
            return len(self)

        if self.operation == Operation.Input:
            ints[param1] = input.get()
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


def intcode(input_str, input=None, output=None):
    ints = list(map(lambda x: int(x), input_str.split(",")))

    if not input:
        input = Queue()

    if not output:
        output = input

    int_pointer = 0
    while int_pointer < len(ints):
        opcode = Opcode(ints[int_pointer])
        if opcode.operation == Operation.Term:
            return list(output.queue)[-1] if output.queue else ints[0]

        int_pointer += opcode.execute(int_pointer, ints, input, output)


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
    input = open('input.txt').readline()

    print(max_thrust(input))
    print(max_thrust_looped(input))
