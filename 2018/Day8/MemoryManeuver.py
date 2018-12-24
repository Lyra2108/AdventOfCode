from itertools import groupby


class Node(object):

    def __init__(self, child_count, metadata_count):
        self.child_count = child_count
        self.metadata_count = metadata_count

        self.parent = None
        self.children = list()
        self.metadata = list()

    def is_parsed(self):
        return len(self.metadata) == self.metadata_count and self.has_enough_children()

    def has_enough_children(self):
        return len(list(filter(lambda child: child.is_parsed(), self.children))) == self.child_count

    def value(self):
        if not any(self.children):
            return sum(self.metadata)

        if not any(self.metadata):
            return 0

        metadata_with_child = list(filter(lambda metadata: 0 < metadata <= len(self.children), self.metadata))
        if not any(metadata_with_child):
            return 0

        grouped_metadata = groupby(sorted(metadata_with_child))

        value = 0
        for child_ref, occurrences in grouped_metadata:
            child_times = len(list(occurrences))
            child = self.children[child_ref - 1]
            value = value + child_times * child.value()
        return value


def pop_header():
    child_count = raw_nodes[0]
    meta_count = raw_nodes[1]
    del raw_nodes[:2]

    return child_count, meta_count


if __name__ == '__main__':
    raw_nodes = list(map(int, open('input.txt', 'r').readline().split(' ')))

    child_count, meta_count = pop_header()
    root_note = Node(child_count, meta_count)
    current_parent = root_note
    nodes = set()
    nodes.add(root_note)
    while not current_parent.is_parsed():
        if current_parent.has_enough_children():
            current_parent.metadata = raw_nodes[:current_parent.metadata_count]
            del raw_nodes[:current_parent.metadata_count]
            if current_parent.parent:
                current_parent = current_parent.parent
        else:
            child_count, meta_count = pop_header()
            child = Node(child_count, meta_count)
            current_parent.children.append(child)
            child.parent = current_parent
            nodes.add(child)

            if child.has_enough_children():
                child.metadata = raw_nodes[:child.metadata_count]
                del raw_nodes[:child.metadata_count]
                if current_parent.has_enough_children():
                    current_parent.metadata = raw_nodes[:current_parent.metadata_count]
                    del raw_nodes[:current_parent.metadata_count]
                    if current_parent.parent:
                        current_parent = current_parent.parent
            else:
                current_parent = child

    print(sum(map(lambda node: sum(node.metadata), nodes)))
    print(root_note.value())
