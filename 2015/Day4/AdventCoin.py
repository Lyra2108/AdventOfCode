from hashlib import md5

if __name__ == "__main__":
    seed = 'iwrupvqb'
    number = 1
    hashstring = (seed + str(number))
    while not md5(hashstring.encode('utf-8')).hexdigest().startswith('000000'):
        number += 1
        hashstring = (seed + str(number))

    print(number)