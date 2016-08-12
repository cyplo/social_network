import time
import subprocess
import os
import platform

@when(u'I launch the client')
def step_impl(context):
    build_configuration = os.environ.get('BUILD_CONFIGURATION') or os.environ.get('CONFIGURATION') or "Release"
    path = "SocialNetworkCLI/bin/%s/SocialNetworkCLI.exe" % build_configuration
    launchers = {"Windows": (path, []), "Linux": ("mono", path)}
    (launcher, arguments) = launchers[platform.system()]
    context.process = subprocess.Popen(args = [launcher, arguments], stdin=subprocess.PIPE, stdout=subprocess.PIPE)

@when(u'I type "exit"')
def step_impl(context):
    context.process.communicate(b"exit\n")

@then(u'the client should quit')
def step_impl(context):
    assert get_returncode(context) != None

@then(u'the client should still be there')
def step_impl(context):
    assert get_returncode(context) == None

@when(u'I wait for 1 second')
def step_impl(context):
    time.sleep(1)

def get_returncode(context):
    context.process.poll()
    return context.process.returncode
