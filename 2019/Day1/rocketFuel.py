from functools import reduce


def read_in_modules():
    input_file = open("input.txt", "r")
    return list(map(lambda x: int(x), input_file.readlines()))


def simple_calculate_fuel(modules):
    return reduce(lambda x, y: x + y, map(lambda module: calculate_fuel(module), modules))


def calculate_fuel(module):
    fuel = int(module / 3) - 2
    return 0 if fuel < 0 else fuel


def calculate_fuel_with_fuel_fuel(modules):
    total_fuel = 0
    fuels = modules
    while fuels:
        fuels = list(filter(lambda module: module > 0, map(lambda module: calculate_fuel(module), fuels)))
        if fuels:
            total_fuel += reduce(lambda x, y: x + y, fuels)

    return total_fuel


if __name__ == '__main__':
    modules = read_in_modules()
    print("The modules need %d fuel." % simple_calculate_fuel(modules))
    print("The modules need %d fuel including their fuel." % calculate_fuel_with_fuel_fuel(modules))
