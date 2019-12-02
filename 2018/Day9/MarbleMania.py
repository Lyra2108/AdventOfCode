from collections import defaultdict


class Marble:
    def __init__(self, number):
        self.number = number
        self.previous = self
        self.next = self

    def add_next(self, number):
        next_marble = Marble(number)
        self.next.previous = next_marble
        next_marble.next = self.next
        self.next = next_marble
        next_marble.previous = self

    def remove(self):
        self.previous.next = self.next
        self.next.previous = self.previous


def marble_mania(player, last_marble):
    current_marble = Marble(0)
    points = defaultdict(lambda: 0)

    for i in range(1, last_marble + 1):
        if i % 23 != 0:
            current_marble.next.add_next(i)
            current_marble = current_marble.next.next
        else:
            pick = current_marble.previous.previous.previous.previous.previous.previous.previous
            points[i % player] += i + pick.number
            current_marble = pick.next
            pick.remove()

    return max(points.values())


if __name__ == '__main__':
    assert 32 == marble_mania(9, 25)
    assert 8317 == marble_mania(10, 1618)
    assert 146373 == marble_mania(13, 7999)
    assert 2764 == marble_mania(17, 1104)
    assert 54718 == marble_mania(21, 6111)
    assert 37305 == marble_mania(30, 5807)

    print("Heighscore: %d" % marble_mania(410, 72059))
    print("Heighscore: %d" % marble_mania(410, 72059*100))
