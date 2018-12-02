from functools import reduce


def read_in_calibrations():
    input_file = open("calibrations.txt", "r")
    return list(map(lambda x: int(x), input_file.readlines()))


def calibrate_device(calibrations):
    return reduce(lambda x, y: x + y, calibrations)


def first_duplicated_state(calibrations):
    states = set()
    state = 0
    iterator = iter(calibrations)
    while True:
        if state in states:
            return state
        states.add(state)
        try:
            state += next(iterator)
        except StopIteration:
            iterator = iter(calibrations)
            state += next(iterator)


if __name__ == '__main__':
    calibrations = read_in_calibrations()
    print("The calibrations sum up to: %d" % calibrate_device(calibrations))
    print("The first duplicated calibration state is %d" % first_duplicated_state(calibrations))
