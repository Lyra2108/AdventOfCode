use itertools::Itertools;
use regex::Regex;
use std::{cmp::Ordering, collections::HashMap, fs};

fn main() {
    let contents: Vec<Point> = fs::read_to_string("input.txt")
        .unwrap()
        .lines()
        .map(Line::new)
        .flatten()
        .sorted_by(|a, b| {
            if Ord::cmp(&b.x, &a.x) != Ordering::Equal {
                Ord::cmp(&b.x, &a.x)
            } else {
                Ord::cmp(&b.y, &a.y)
            }
        })
        .collect();

    let mut grouped_list = HashMap::new();
    for point in &contents {
        grouped_list
            .entry(point)
            .or_insert_with(Vec::new)
            .push(point)
    }

    let mut count = 0;
    for points in grouped_list.values() {
        if points.len() > 1 {
            count += 1;
        }
    }

    println!("There are {} points which are hit at least twice!", count);
}

pub struct Line {
    start: Point,
    end: Point,
    done: bool,
}

#[derive(Eq, PartialEq, Hash, Copy, Clone)]
pub struct Point {
    x: i32,
    y: i32,
}

impl Line {
    pub fn new(line: &str) -> Line {
        let line_regex: Regex =
            Regex::new(r"(?P<startx>\d+),(?P<starty>\d+) -> (?P<endx>\d+),(?P<endy>\d+)").unwrap();
        let captures = line_regex.captures(line).unwrap();
        Line {
            start: Point {
                x: captures["startx"].parse().unwrap(),
                y: captures["starty"].parse().unwrap(),
            },
            end: Point {
                x: captures["endx"].parse().unwrap(),
                y: captures["endy"].parse().unwrap(),
            },
            done: false,
        }
    }
}

impl Iterator for Line {
    type Item = Point;

    fn next(&mut self) -> Option<<Self as Iterator>::Item> {
        if self.done {
            None
        } else if self.start.x == self.end.x && self.start.y == self.end.y {
            self.done = true;
            Some(self.start)
        } else {
            let result = Some(self.start);
            let x_offset: i32 = if self.start.x == self.end.x {
                0
            } else if self.start.x < self.end.x {
                1
            } else {
                -1
            };
            let y_offset: i32 = if self.start.y == self.end.y {
                0
            } else if self.start.y < self.end.y {
                1
            } else {
                -1
            };

            self.start = Point {
                x: self.start.x + x_offset,
                y: self.start.y + y_offset,
            };
            result
        }
    }
}
