if __name__ == '__main__':
    navigation = open('flores.txt', 'r').readline()
    downStairs = navigation.count(')')
    upStairs = navigation.count('(')
    floors = (upStairs - downStairs)
    print("He needs to go %d" % floors)

    currentFloor = 0
    for i, navigation_step in enumerate(navigation):
        if currentFloor < 0:
            print("He entered the basement in step %d" % i)
            break

        currentFloor += 1 if navigation_step == "(" else -1
