@given(u'I\'ve already posted as Alice')
def step_impl(context):
    context.execute_steps(u"""When I type "Alice -> Some previous text by Alice" """)
