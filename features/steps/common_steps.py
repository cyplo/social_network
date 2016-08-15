import time
import os
from behave import *

@when(u'I type "{text}"')
@when(u'They type "{text}"')
def step_impl(context, text):
    text_to_write = text + os.linesep
    context.process.stdin.write(bytearray(text_to_write, 'utf-8'))

@then(u'the prompt should still be there')
def step_impl(context):
    return_code = get_returncode(context)
    assert return_code == None
	
@when(u'I wait for 1 second')
def step_impl(context):
    time.sleep(1)

@when(u'I pass the keyboard to another person')
# this is an interaction between people, not necessarily concerning the application itself 
def step_impl(context):
    pass

def get_returncode(context):
    context.process.poll()
    return context.process.returncode
