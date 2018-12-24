import re


class Constraint:
    def __init__(self, constraint_str):
        constraint_match = re.match(r"Step (?P<parent>.) must be finished before step (?P<child>.) can begin.",
                                    constraint_str)
        self.child = constraint_match.group('child')
        self.parent = constraint_match.group('parent')


class Task(object):

    def __init__(self, name):
        self.name = name
        self.parents = set()
        self.children = set()
        self.time = 60 + (ord(name) - ord('A'))

    def can_be_done(self):
        return not any(self.parents)


def calculate_work_order(tasks):
    work_order = ''
    do_able = sorted(set(filter(lambda task: task.can_be_done(), tasks.values())), key=lambda task: task.name)
    while any(do_able):
        to_be_done = do_able[0]

        work_order = work_order + to_be_done.name
        del tasks[to_be_done.name]
        for child in to_be_done.children:
            child.parents.remove(to_be_done)

        do_able = sorted(set(filter(lambda task: task.can_be_done(), tasks.values())), key=lambda task: task.name)
    return work_order


def calculate_work_time(tasks):
    time = 0
    work_slots = [None, None, None, None, None]
    do_able = sorted(set(filter(lambda node: node.can_be_done(), tasks.values())), key=lambda task: task.name)
    while any(do_able):
        not_started = sorted(set(do_able) - set(work_slots), key=lambda node: node.name)
        to_be_done = iter(not_started[0:min(work_slots.count(None), len(not_started))])

        time = time + 1
        for i in range(0, len(work_slots)):
            current_task = work_slots[i]
            if not current_task:
                next_task = next(to_be_done, None)
                work_slots[i] = next_task
            else:
                if current_task.time > 0:
                    current_task.time = current_task.time - 1
                if current_task.time == 0:
                    work_slots[i] = None
                    del tasks[current_task.name]
                    for child in current_task.children:
                        child.parents.remove(current_task)

        do_able = sorted(set(filter(lambda task: task.can_be_done(), tasks.values())), key=lambda task: task.name)

    return time


def read_in_tasks():
    conditions = list(map(lambda constraint_str: Constraint(constraint_str),
                          open('input.txt', 'r').readlines()))
    nodes = dict()
    for condition in conditions:
        if condition.child in nodes.keys():
            child = nodes[condition.child]
        else:
            child = Task(condition.child)
            nodes[condition.child] = child

        if condition.parent in nodes.keys():
            parent = nodes[condition.parent]
        else:
            parent = Task(name=condition.parent)
            nodes[condition.parent] = parent

        child.parents.add(parent)
        parent.children.add(child)
    return nodes


if __name__ == '__main__':
    print(calculate_work_order(read_in_tasks()))
    print(calculate_work_time(read_in_tasks()))
