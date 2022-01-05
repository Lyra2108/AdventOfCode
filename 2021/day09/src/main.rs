use std::fs;

fn main() {
    let contents: Vec<Vec<u8>> = fs::read_to_string("input.txt")
        .unwrap()
        .lines()
        .map(|x| {
            x.chars()
                .map(|letter| letter.to_string().parse().unwrap())
                .collect()
        })
        .collect();

    let mut risk = 0u32;
    for (y, line) in contents.iter().enumerate() {
        for (x, number) in line.iter().enumerate() {
            let y_lower = if y == 0 { 0 } else { y - 1 };
            let y_upper = if y + 1 == line.len() { y } else { y + 1 };
            let x_lower = if x == 0 { 0 } else { x - 1 };
            let x_upper = if x + 1 == line.len() { x } else { x + 1 };

            let mut is_lowest = true;
            for other_y in y_lower..=y_upper {
                for other_x in x_lower..=x_upper {
                    if other_x == x && other_y == y {
                        continue;
                    }

                    if number >= &contents[other_y][other_x] {
                        is_lowest = false;
                    }
                }
            }

            if is_lowest {
                risk += u32::from(*number) + 1;
            }
        }
    }

    println!("The risk is {}", risk);
}
