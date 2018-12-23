import re
from collections import defaultdict
from datetime import datetime, timedelta


class Guard:

    def __init__(self, guard_id):
        self.id = guard_id

        self.total_slept = timedelta()
        self.minutes = defaultdict(int)

    def log_sleep(self, start, end):
        sleep_time = end - start
        total_min = int(sleep_time.total_seconds() / 60)
        self.total_slept = self.total_slept + timedelta(minutes=total_min)
        for minute in range(start.time().minute, start.time().minute + total_min):
            self.minutes[minute % 60] = self.minutes[minute % 60] + 1

    def most_slept_min(self):
        if not self.minutes:
            return 0, 0
        most_slept_min = max(self.minutes, key=lambda i: self.minutes[i])
        return most_slept_min, self.minutes[most_slept_min]


if __name__ == '__main__':
    records = list(sorted(
        map(lambda regex: (datetime.fromisoformat(regex.group('time')), regex.group('text')),
            map(lambda record: re.match(r"^\[(?P<time>.+)\]\s(?P<text>.+)$", record),
                open('input.txt', 'r').readlines()
                )), key=lambda record: record[0]))

    guards = dict()
    guard_line = re.compile(r"Guard #(?P<id>\d+) begins shift")
    start_line = "falls asleep"
    end_line = "wakes up"

    for time, text in records:
        guard_match = guard_line.match(text)
        if guard_match:
            guard_id = int(guard_match.group('id'))
            if guard_id not in guards.keys():
                guards[guard_id] = Guard(guard_id)
            current_guard = guards[guard_id]

        if text == start_line:
            start_time = time

        if text == end_line:
            current_guard.log_sleep(start_time, time)

    most_sleepy = guards[max(guards, key=lambda i: guards[i].total_slept)]
    print(most_sleepy.id * most_sleepy.most_slept_min()[0])

    most_sleepy_at_min = guards[max(guards, key=lambda i: guards[i].most_slept_min()[1])]
    print(most_sleepy_at_min.id * most_sleepy_at_min.most_slept_min()[0])
