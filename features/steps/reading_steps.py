from behave import *

@then(u'I should see "{text}"')
def step_impl(context, text):
    output = check_process_output(context)
    if output:
        assert text in output
    else:
        context.check_queue.put(text)

def check_process_output(context):
    if context.stdout_queue.empty():
        return None
    output = context.stdout_queue.get()
    return output


