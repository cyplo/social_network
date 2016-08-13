import subprocess
import os
import platform
from behave import *

def get_returncode(context):
    context.process.poll()
    return context.process.returncode

def before_scenario(context, scenario):
    build_configuration = os.environ.get('BUILD_CONFIGURATION') or os.environ.get('CONFIGURATION') or "Release"
    path = "SocialNetworkCLI/bin/%s/SocialNetworkCLI.exe" % build_configuration
    launchers = {"Windows": (path, []), "Linux": ("mono", path)}
    (launcher, arguments) = launchers[platform.system()]
    context.process = subprocess.Popen(args = [launcher, arguments], stdin=subprocess.PIPE, stdout=subprocess.PIPE)

def after_scenario(context, scenario):
    if get_returncode(context) == None:
        exit_text = "quit%s" % os.linesep
        (output,_) = context.process.communicate(bytearray(exit_text, 'utf-8'), timeout = 5)
    if get_returncode(context) == None:
        context.process.kill()
