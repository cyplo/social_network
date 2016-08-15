from behave import *

@given(u'{user1} follows {user2}')
@when(u'{user1} follows {user2}')
def step_impl(context, user1, user2):
    context.execute_steps(u"When I type \"" + user1 + " follows " + user2 + "\"")