def calculate_package_needs(boxes):
    paper = 0
    ribbon = 0
    for box in boxes:
        sizes = list(map(lambda size: int(size), box))
        x, y, z = sizes
        sizes.remove(max(sizes))
        x_small, y_small = sizes
        paper += 2*x*y + 2*x*z + 2*y*z + x_small*y_small
        ribbon += 2*x_small + 2*y_small + x*y*z
    return (paper, ribbon)


if __name__ == "__main__":
    raw_boxes = open("sizes.txt", "r").readlines()
    boxes = map(lambda size: size.split('x'), raw_boxes)
    print("They need %d square foot of wrapping paper and %d foot of ribbon." %
          calculate_package_needs(boxes))
