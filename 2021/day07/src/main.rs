use std::fs;

fn main() {
    let crabs: Vec<i32> = fs::read_to_string("input.txt")
        .unwrap()
        .lines()
        .map(|line| line.split(','))
        .flatten()
        .map(|number| number.parse().unwrap())
        .collect();

    let min = *crabs.iter().min().unwrap();
    let max = *crabs.iter().max().unwrap();

    let mut fuel = i32::MAX;
    for number in min..=max {
        let current_cost : i32 = crabs.iter().map(|crab| (*crab - number).abs()).sum();
        if current_cost < fuel{
            fuel = current_cost;
        }
    }
    println!("The minimal cost is {}", fuel);

    let mut fuel = i32::MAX;
    for number in min..=max {
        let current_cost : i32 = crabs.iter().map(|crab| (1..=(*crab - number).abs()).sum::<i32>()).sum();
        if current_cost < fuel{
            fuel = current_cost;
        }
    }
    println!("The minimal cost is {}", fuel)
}
