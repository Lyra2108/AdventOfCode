use std::fs;

fn main() {
    let contents: Vec<i16> = fs::read_to_string("input.txt")
        .unwrap()
        .lines()
        .map(|x| x.parse().unwrap())
        .collect();

    let task_a: i16 = contents
        .iter()
        .skip(1)
        .zip(contents.iter())
        .fold(0, |count, touple| {
            if touple.1 < touple.0 {
                count + 1
            } else {
                count
            }
        });

    println!("Task A:\n{}", task_a);

    let task_b: i16 = contents
        .iter()
        .skip(3)
        .zip(contents.iter())
        .fold(0, |count, (second, first)| {
            match first < second
            {
                true => count + 1,
                false => count
            }
        });

    println!("Task B:\n{}", task_b);
}
