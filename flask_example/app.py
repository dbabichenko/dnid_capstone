from flask import Flask, render_template, request
from openai import OpenAI

app = Flask(__name__)

gpt_key = 'YOUR API KEY HERE'


@app.route('/')
def home():
    return "Welcome to My Flask App!"

@app.route('/dnid_capstone')
def dnid_capstone():
    return "Hello and welcome to DNID capstone"

@app.route('/hello')
def hello():
    return render_template('hello.html')

@app.route('/form')
def form():
    return render_template('form.html')

@app.route('/get_product')
def get_product():
    product_id = request.args.get('product_id')
    if product_id == None or product_id == "":
        return "You must provide a product id"
    else:
        if str(product_id) == "1":
            return "You just bought a horse"
        elif str(product_id) == "2":
            return "You just bought a house"
        else:
            return "You did not buy anything"
        

@app.route('/process_prompt', methods=['POST'])
def process_prompt():
    prompt = request.form.get('txtPrompt')
    # Instantiate OpenAI Client
    client = OpenAI(
        api_key=gpt_key,  
    )
    completion = client.chat.completions.create(model="gpt-4-turbo", messages=[{"role": "user", "content": prompt}])
    return completion.choices[0].message.content

if __name__ == '__main__':
    app.run(debug=True)
