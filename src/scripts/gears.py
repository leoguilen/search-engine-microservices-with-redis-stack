import json

def load_autocomplete(x, field):
    values = x['value'][field].split(" ")
    for v in values:
        if len(v) > 3:
            execute('FT.SUGADD', 'articles:autocomplete', v, 1, 'PAYLOAD', json.dumps(x))

def process(x):
    streamId = x['id'][:-2]
    index = 'article:' + streamId
    with atomic():
        execute('JSON.SET', index, '$', json.dumps(x))
        load_autocomplete(x, 'title')
        load_autocomplete(x, 'summary')

def main():
    gb = GB('StreamReader')
    gb.map(process)
    gb.register('extracted-articles-stream')

main()