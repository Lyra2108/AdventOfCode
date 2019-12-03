class Direction(object):
    def __init__(self, direction_string):
        self.direction = direction_string[0]
        self.steps = int(direction_string[1:])


class Line(object):
    def __init__(self, start, end, direction, distance_from_start):
        self.distance_from_start = distance_from_start
        self.direction = direction
        self.start = start
        self.end = end

    def intersects(self, line):
        x_direction = ("D", "U")
        if line.direction in x_direction and self.direction in x_direction:
            return
        y_direction = ("L", "R")
        if line.direction in y_direction and self.direction in y_direction:
            return

        vertical_line = line if line.direction in x_direction else self
        horizontal_line = line if line.direction in y_direction else self

        x_start = min(vertical_line.start[0], vertical_line.end[0])
        x_end = max(vertical_line.start[0], vertical_line.end[0])
        if not x_start < horizontal_line.start[0] < x_end:
            return

        y_start = min(horizontal_line.start[1] , horizontal_line.end[1])
        y_end = max(horizontal_line.start[1], horizontal_line.end[1])
        if not y_start < vertical_line.start[1] < y_end:
            return

        total_distance = horizontal_line.distance_from_start \
                         + vertical_line.distance_from_start \
                         + abs(horizontal_line.start[0] - vertical_line.start[0]) \
                         + abs(vertical_line.start[1] - horizontal_line.start[1])
        return horizontal_line.start[0], vertical_line.start[1], total_distance


class Wire:
    def __init__(self, wire_route):
        self.directions = list(map(lambda direction: Direction(direction), wire_route.split(",")))
        self.lines = self.set_up_lines()

    def set_up_lines(self):
        lines = []
        point = (0, 0)
        distance_from_start = 0
        for direction in self.directions:
            new_point = None
            if direction.direction == "D":
                new_point = (point[0] - direction.steps, point[1])
            elif direction.direction == "U":
                new_point = (point[0] + direction.steps, point[1])
            elif direction.direction == "L":
                new_point = (point[0], point[1] - direction.steps)
            elif direction.direction == "R":
                new_point = (point[0], point[1] + direction.steps)
            lines.append(Line(point, new_point, direction.direction, distance_from_start))
            point = new_point
            distance_from_start += direction.steps

        return lines

    def intersects(self, wire):
        intersections = []
        for own_line in self.lines:
            for other_line in wire.lines:
                intersection = own_line.intersects(other_line)
                if intersection and intersection != (0, 0, 0):
                    intersections.append((abs(intersection[0]) + abs(intersection[1]), intersection[2]))

        manhattan = min(map(lambda inter: inter[0], intersections))
        steps = min(map(lambda inter: inter[1], intersections))
        return manhattan, steps


def wires_crossing(wire_routs):
    wires = list(map(lambda wire_route: Wire(wire_route), wire_routs))

    return wires[0].intersects(wires[1])


if __name__ == '__main__':
    assert (6, 30) == wires_crossing(["R8,U5,L5,D3", "U7,R6,D4,L4"])
    assert (159, 610) == wires_crossing(["R75,D30,R83,U83,L12,D49,R71,U7,L72", "U62,R66,U55,R34,D71,R55,D58,R83"])
    assert (135, 410) == wires_crossing(
        ["R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51", "U98,R91,D20,R16,D67,R40,U7,R15,U6,R7"])

    print(wires_crossing(open('input.txt').readlines()))
