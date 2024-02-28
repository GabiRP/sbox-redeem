import discord
import re
import requests
import os
pattern = re.compile(r"[0-9A-Fa-f]{8}-[0-9A-Fa-f]{4}-[0-9A-Fa-f]{4}-[0-9A-Fa-f]{4}-[0-9A-Fa-f]{12}", re.IGNORECASE)

def get_token():
    if(not os.path.isfile("token.txt")):
        print("Create token.txt file and paste your token")
        exit()
    file = open("token.txt")
    return file.read()

def check_regex(input):
        return pattern.match(input)
def get_regex(input):
    if re.search(pattern, input) == None:
        return ""
    return re.search(pattern, input).group(0)
api_url = "http://192.168.1.193:5002/"
def send_post(key):
    response = requests.post(api_url+key)
    print(response._content)

class Client(discord.Client):
    async def on_ready(self):
        print(f'We have logged in as {self.user}')

    async def on_message(self, message):
        string = get_regex(message.content)
        if len(string) == 0:
            return
        print(f'message from {message.author}: {string}')
        send_post(string)



client = Client()
client.run(get_token())