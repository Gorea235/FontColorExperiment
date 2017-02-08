import json

FILE_PATH = "UserAnswers.json"
CSV_TIMES_OUT = "times.csv"
CSV_PREF_OUT = "preferences.csv"
USER_SKIP = [
    "3d8dc1d8-3e09-4bd0-84b0-a4b5f40ca56d"
]

def load_data(file):
    with open(file) as fp:
        return json.load(fp)


def build_string(strlist, join):
    return join.join(strlist)


def get_id(k1, k2):
    return "{}-{}".format(k1, k2)


def output_csv(lines, path):
    csv_full = build_string(lines, "\n")
    #print(csv_full)
    with open(path, "w") as fp:
        fp.write(csv_full)


if __name__ == "__main__":
    data = load_data(FILE_PATH)
    out = []
    times = {}
    time_lines = []
    pref_lines = []
    item_headers = []
    pref_headers = []
    items = {}
    prefs = {}
    for user in data:
        if user["Id"] in USER_SKIP:
            continue
        item_headers = []
        for k1, v1 in user["Answers"].items():
            for k2, v2 in v1.items():
                out.append("User ({}) {}: {}".format(user["Id"], get_id(k1, k2), v2["TimeTaken"]))
                id = get_id(k1, k2)
                if not id in times:
                    times[id] = []
                item_headers.append(id)
                times[id].append(v2["TimeTaken"])
                items[id] = v2["TimeTaken"].split(":")[-1]
        pref_headers = []
        for k, v in user["FinalAnswers"].items():
            pref_headers.append(k)
            prefs[k] = v
        item_headers.sort()
        pref_headers.sort()
        line = []
        for head in item_headers:
            line.append(items[head])
        time_lines.append(build_string(line, ","))
        line = []
        for head in pref_headers:
            line.append(str(prefs[head]))
        pref_lines.append(build_string(line, ","))
        #print(line)
        #print(time_lines)

    print(build_string(out, "\n"))
    print("Displaying times for {} users".format(len(data)))
    item_headers.sort()
    time_lines.insert(0, build_string(item_headers, ","))
    pref_headers.sort()
    pref_lines.insert(0, build_string(pref_headers, ","))
    output_csv(time_lines, CSV_TIMES_OUT)
    output_csv(pref_lines, CSV_PREF_OUT)
