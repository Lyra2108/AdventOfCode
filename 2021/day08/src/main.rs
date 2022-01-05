use std::collections::HashMap;
use std::fs;

fn main() {
    let input = fs::read_to_string("input.txt").unwrap();
    let contents: Vec<&str> = input
        .lines()
        .map(|line| {
            line.split_once("|")
                .unwrap()
                .1
                .split(' ')
                .collect::<Vec<&str>>()
        })
        .flatten()
        .collect();

    // let abc = Regex::new(r"^.+\|( ((?P<one>\w{2})|(?P<four>\w{4})|(?P<seven>\w{3})|(?P<eight>\w{7})|\w+))+$").unwrap();

    let mut counter: HashMap<usize, u128> = HashMap::new();
    for elem in contents {
        *counter.entry(elem.chars().count()).or_insert(0) += 1;
    }

    println!(
        "The sum of the numbers is {}",
        *counter.entry(2).or_default()
            + *counter.entry(4).or_default()
            + *counter.entry(3).or_default()
            + *counter.entry(7).or_default()
    );

    let abc: Vec<(Vec<&str>, Vec<&str>)> = input
        .lines()
        .map(|line| line.split_once("| ").unwrap())
        .map(|(numbers, target)| {
            (
                numbers.split(' ').collect::<Vec<&str>>(),
                target.split(' ').collect::<Vec<&str>>(),
            )
        })
        .collect();

    let mut final_sum = 0u32;
    for (numbers, target) in abc {
        let mut number_map: HashMap<u16, &str> = HashMap::new();

        let number_slice = &numbers[..];
        for number in number_slice {
            let length = number.chars().count();
            match length {
                2 => {
                    number_map.insert(1, number);
                }
                3 => {
                    number_map.insert(7, number);
                }
                4 => {
                    number_map.insert(4, number);
                }
                7 => {
                    number_map.insert(8, number);
                }
                _ => {}
            }
        }

        for number in number_slice {
            let length = number.chars().count();
            if length == 6 {
                if number_map
                    .get(&4)
                    .unwrap()
                    .chars()
                    .into_iter()
                    .all(|letter| number.contains(letter))
                {
                    number_map.insert(9, number);
                } else if number_map
                    .get(&1)
                    .unwrap()
                    .chars()
                    .into_iter()
                    .all(|letter| number.contains(letter))
                {
                    number_map.insert(0, number);
                } else {
                    number_map.insert(6, number);
                }
            }
        }

        let upper_right = number_map
            .get(&8)
            .unwrap()
            .chars()
            .find(|letter| !(*number_map.get(&6).unwrap()).contains(*letter))
            .unwrap();

        for number in number_slice {
            let length = number.chars().count();
            if length == 5 {
                if number_map
                    .get(&7)
                    .unwrap()
                    .chars()
                    .into_iter()
                    .all(|letter| number.contains(letter))
                {
                    number_map.insert(3, number);
                } else if number.contains(upper_right) {
                    number_map.insert(2, number);
                } else {
                    number_map.insert(5, number);
                }
            }
        }

        let mut char_map: HashMap<&str, u16> = HashMap::new();
        for (key, value) in number_map {
            char_map.insert(value, key);
        }

        let mut decoded_number = 0u16;
        for number in target {
            let key = char_map
                .keys()
                .find(|key| {
                    number.chars().count() == key.chars().count()
                        && number.chars().all(|letter| key.contains(letter))
                })
                .unwrap();

            decoded_number = decoded_number * 10 + char_map.get(key).unwrap();
        }

        final_sum += u32::from(decoded_number);
    }

    println!("The final sum is {}", final_sum);
}
