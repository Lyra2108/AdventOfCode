from pprint import pprint


def navigate(position, navigation):
    if navigation == "<":
        return (position[0] - 1, position[1])
    if navigation == ">":
        return (position[0] + 1, position[1])
    if navigation == "v":
        return (position[0], position[1] - 1)
    if navigation == "^":
        return (position[0], position[1] + 1)


def santa_only(navigations):
    position = (0, 0)
    visited = set()
    visited.add(position)
    for i, navigation in enumerate(navigations):
        position = navigate(position, navigation)
        visited.add(position)
    return len(visited)


def santa_and_robo(navigations):
    santa = (0, 0)
    robo = (0, 0)
    visited = set()
    visited.add(santa)
    for i, navigation in enumerate(navigations):
        if i % 2 == 0:
            santa = navigate(santa, navigation)
            visited.add(santa)
        else:
            robo = navigate(robo, navigation)
            visited.add(robo)

    return len(visited)


if __name__ == "__main__":
    navigations = open("navigations.txt", "r").readline()

    print(santa_only(navigations))
    print(santa_and_robo(navigations))
