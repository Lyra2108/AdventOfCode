use std::collections::HashMap;
use std::fs;

fn main() {
    let contents: Vec<u128> = fs::read_to_string("input.txt")
        .unwrap()
        .lines()
        .map(|line| line.split(','))
        .flatten()
        .map(|number| number.parse().unwrap())
        .collect();

    let mut grouped_fish: HashMap<u128, u128> = HashMap::new();
    for fish in &contents {
        *grouped_fish.entry(*fish).or_insert(0) += 1;
    }

    for _n in 1..=256 {
        let mut new_fish: HashMap<u128, u128> = HashMap::new();
        for (stage, number_of_fish) in grouped_fish {
            match stage {
                0 => {
                    *new_fish.entry(6).or_insert(0) += number_of_fish;
                    *new_fish.entry(8).or_insert(0) += number_of_fish
                }
                1..=8 => *new_fish.entry(stage - 1).or_insert(0) += number_of_fish,
                _ => panic!("recived an unsupported value"),
            };
        }
        grouped_fish = new_fish.clone();
    }

    println!(
        "There are {} fish",
        grouped_fish.values().into_iter().sum::<u128>()
    );
}
