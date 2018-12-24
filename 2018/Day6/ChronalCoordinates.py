import re
from math import sqrt


class Point:

    def __init__(self, x, y):
        self.x = int(x)
        self.y = int(y)
        self.area = 0

    def distance(self, other):
        return sqrt((self.x - other.x)**2) + sqrt((self.y - other.y)**2)


def find_closest_point(x, y, coordinates):
    closest_distance = None
    closest_point = None
    for point in coordinates:
        distance = sqrt((x - point.x)**2) + sqrt((y - point.y)**2)

        if distance == closest_distance:
            twice = True
            continue

        if not closest_point or distance < closest_distance:
            closest_distance = distance
            closest_point = point
            twice = False

    if twice:
        return None

    return closest_point


if __name__ == '__main__':
    coordinates = list(
        map(lambda matched_line: Point(matched_line.group('x'), matched_line.group('y')),
            map(lambda line: re.match(r"(?P<x>\d+), (?P<y>\d+)", line),
                open('input.txt', 'r').readlines())))

    upper_left_corner = Point(min(coordinates, key=lambda point: point.x).x, min(coordinates, key=lambda point: point.y).y)
    lower_right_corner = Point(max(coordinates, key=lambda point: point.x).x, max(coordinates, key=lambda point: point.y).y)

    for x in range(upper_left_corner.x, lower_right_corner.x):
        for y in range(upper_left_corner.y, lower_right_corner.y):
            close_point = find_closest_point(x, y, coordinates)
            if close_point:
                close_point.area = close_point.area + 1

    print(max(coordinates, key=lambda point: point.area).area)

    limit = 10000
    close_area = 0
    for x in range(upper_left_corner.x, lower_right_corner.x):
        for y in range(upper_left_corner.y, lower_right_corner.y):
            distance = 0
            for point in coordinates:
                distance = distance + point.distance(Point(x, y))
                if distance >= limit:
                    break
            if distance < limit:
                close_area = close_area + 1

    print(close_area)
