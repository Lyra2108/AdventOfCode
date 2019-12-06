from functools import reduce


class Orbit(object):

    def __init__(self, name, parent=None):
        self.parent = parent
        self.name = name

    def __eq__(self, other):
        if isinstance(other, str):
            return self.name == other

        return self.name == other.name

    def __hash__(self):
        return hash(self.name)

    @property
    def length(self):
        if self.parent:
            return self.parent.length + 1
        else:
            return 0

    @property
    def parents(self):
        if self.parent:
            return self.parent.parents | {self.parent}
        else:
            return set()


def read_in_orbits(orbit_instructions):
    orbits = []
    for orbit_instruction in orbit_instructions:
        parent, child = orbit_instruction.split(')')
        if parent in orbits:
            parent = orbits[orbits.index(parent)]
        else:
            parent = Orbit(parent)
            orbits.append(parent)

        child = child.strip()
        if child in orbits:
            child = orbits[orbits.index(child)]
            child.parent = parent
        else:
            orbits.append(Orbit(child, parent))
    return orbits


def orbit_count(orbits):
    return reduce(lambda x, y: x + y, map(lambda orbit: orbit.length, orbits))


def distance_to_santa(orbits):
    you = orbits[orbits.index('YOU')]
    santa = orbits[orbits.index('SAN')]

    not_common_orbits = you.parents ^ santa.parents
    return len(not_common_orbits)


if __name__ == '__main__':
    example_orbits = read_in_orbits(open('example.txt').readlines())
    assert 54 == orbit_count(example_orbits)
    assert 4 == distance_to_santa(example_orbits)

    input_orbits = read_in_orbits(open('input.txt').readlines())
    print(orbit_count(input_orbits))
    print(distance_to_santa(input_orbits))
