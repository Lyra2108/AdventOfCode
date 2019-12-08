from collections import Counter


class Layer(object):
    def __init__(self, ints):
        self.ints = ints
        self.counts = Counter(ints)

    @property
    def zero(self):
        return self.counts[0]

    @property
    def one(self):
        return self.counts[1]

    @property
    def two(self):
        return self.counts[2]


class Image(object):
    def __init__(self, ints, width, height):
        ints = list(map(lambda x: int(x), ints))

        self.width = width
        self.height = height
        self.layers = []
        for i in range(0, len(ints), width * height):
            self.layers.append(Layer(ints[i:i + width * height]))

    def find_pixel(self, x, y):
        for layer in range(0, len(self.layers)):
            pixel = self.layers[layer].ints[x + y]
            if pixel in (0, 1):
                return pixel
        return pixel

    def __str__(self):
        output = ''
        for y in range(0, self.width * self.height, self.width):
            for x in range(0, self.width):
                pixel = self.find_pixel(x, y)
                output += str(pixel)
            output += "\n"
        return output.replace('0', '.').replace('1', '#')


def checksum_space_image(input, width, height):
    image = Image(input, width, height)
    min_zero = min(image.layers, key=lambda layer: layer.zero)

    return min_zero.one * min_zero.two


if __name__ == '__main__':
    assert 1 == checksum_space_image("123456789012", 3, 2)
    print(Image("0222112222120000", 2, 2))

    puzzle_input = open('input.txt').readline().strip()
    print(checksum_space_image(puzzle_input, 25, 6))
    print(Image(puzzle_input, 25, 6))
