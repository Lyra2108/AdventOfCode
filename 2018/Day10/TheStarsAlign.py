import re
from collections import defaultdict


class Star:

    def __init__(self, start_str):
        star_match = re.match(
            r"position=<\s*(?P<x>-?\d+),\s*(?P<y>-?\d+)> velocity=<\s*(?P<x_speed>-?\d+),\s*(?P<y_speed>-?\d+)>",
            start_str)
        self.x = int(star_match.group("x"))
        self.y = int(star_match.group("y"))
        self.speed_x = int(star_match.group("x_speed"))
        self.speed_y = int(star_match.group("y_speed"))

    def move(self):
        self.x = self.x + self.speed_x
        self.y = self.y + self.speed_y

    def has_neighbor(self, other_stars):
        for star in other_stars:
            if star.is_neighbor(self):
                return True

        return False

    def is_neighbor(self, other_star):
        return self.x - 1 <= other_star.x <= self.x + 1 and self.y - 1 <= other_star.y <= self.y + 1


if __name__ == '__main__':
    stars = list(map(Star, open('input.txt', 'r').readlines()))

    seconds = 0
    while not all(map(lambda star: star.has_neighbor(filter(lambda filter_star: filter_star != star, stars)), stars)):
        for star in stars:
            star.move()
        seconds = seconds + 1

    star_map = defaultdict(list)
    for star in stars:
        star_map[star.x].append(star.y)

    min_x = min(star_map.keys())
    max_x = max(star_map.keys())
    min_y = min(map(lambda star: star.y, stars))
    max_y = max(map(lambda star: star.y, stars))

    output = ''
    for y in range(min_y, max_y + 1):
        for x in range(min_x, max_x + 1):
            if y in star_map[x]:
                output = output + '#'
            else:
                output = output + '.'
        output = output + '\n'

    print(output)
    print(seconds)
