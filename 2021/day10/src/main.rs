use std::fs;

fn main() {
    let input = fs::read_to_string("input.txt").unwrap();

    let mut errors = 0u32;
    let mut compleation_cost: Vec<u128> = Vec::new();
    let opening_braces = ['(', '<', '{', '['];
    for line in input.lines() {
        let mut stack: Vec<char> = Vec::new();
        let mut incomplete = true;
        for brace in line.chars() {
            if opening_braces.contains(&brace) {
                stack.push(brace);
            } else {
                let last_opening_brace = stack.pop().unwrap();
                match brace {
                    ')' => {
                        if last_opening_brace != '(' {
                            errors += 3;
                            incomplete = false;
                            break;
                        }
                    }
                    ']' => {
                        if last_opening_brace != '[' {
                            errors += 57;
                            incomplete = false;
                            break;
                        }
                    }
                    '>' => {
                        if last_opening_brace != '<' {
                            errors += 25137;
                            incomplete = false;
                            break;
                        }
                    }
                    '}' => {
                        if last_opening_brace != '{' {
                            errors += 1197;
                            incomplete = false;
                            break;
                        }
                    }
                    _ => {}
                }
            }
        }

        if incomplete {
            let mut line_cost = 0u128;

            stack.reverse();
            for brace in stack {
                let cost = match brace {
                    '(' => 1,
                    '[' => 2,
                    '{' => 3,
                    '<' => 4,
                    _ => panic!("should not happen"),
                };
                line_cost = line_cost * 5 + cost;
            }
            compleation_cost.push(line_cost);
        }
    }

    println!("The error count is {}", errors);

    compleation_cost.sort_unstable();

    println!(
        "The correction costs are {}",
        compleation_cost[compleation_cost.len() / 2]
    );
}
