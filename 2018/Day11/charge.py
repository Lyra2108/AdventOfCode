class Grid:
    def __init__(self, serial_number):
        self.grid = list()
        self.cache = dict()
        for x in range(1, 300 + 1):
            self.grid.append(list())
            for y in range(1, 300 + 1):
                rack_id = x + 10
                power_level = int('{:0>3}'.format((rack_id * y + serial_number) * rack_id)[-3]) - 5
                self.grid[x - 1].append(power_level)

    def power_at(self, x, y):
        return self.grid[x - 1][y - 1]

    def power_in_grid(self, x, y, size):
        sum = 0
        if self.cache.__contains__((x, y, size - 1)):
            sum = self.cache[(x, y, size-1)]
            for grid_x in range(x - 1, x + size - 1):
                value = self.grid[grid_x][y + size - 2]
                sum += value
            for grid_y in range(y - 1, y + size - 1):
                valuey = self.grid[x + size - 2][grid_y]
                sum += valuey
            del self.cache[(x, y, size - 1)]
            self.cache[(x, y, size)] = sum
        else:
            for grid_x in range(x - 1, x + size - 1):
                for grid_y in range(y - 1, y + size - 1):
                    sum += self.grid[grid_x][grid_y]
            self.cache[(x, y, size)] = sum
        return sum


def find_max(grid, size=3):
    max = (0, 0, 0)
    for x in range(1, 300 - size):
        for y in range(1, 300 - size):
            value = grid.power_in_grid(x, y, size)
            if value > max[0]:
                max = (value, x, y)
    return max


if __name__ == '__main__':
    assert 4 == Grid(8).power_at(3, 5)
    assert -5 == Grid(57).power_at(122, 79)
    assert 0 == Grid(39).power_at(217, 196)
    assert 4 == Grid(71).power_at(101, 153)

    assert (29, 33, 45) == find_max(Grid(18))

    print(find_max(Grid(8979)))

    grid = Grid(8979)
    max = (0, 0, 0, 0)
    for size in range(1, 301):
        max_power = find_max(grid, size)
        if max_power[0] > max[0]:
            max = (max_power[0], max_power[1], max_power[2], size)

    print(max)
