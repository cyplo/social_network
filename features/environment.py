import subprocess
import os
import platform
import queue
import threading

def get_returncode(context):
    context.process.poll()
    return context.process.returncode

def blocking_read(context):
    try:
        for line in context.process.stdout:
            context.stdout_queue.put_nowait(line)
    except:
        pass

def before_scenario(context, scenario):
    build_configuration = os.environ.get('BUILD_CONFIGURATION') or os.environ.get('CONFIGURATION') or "Release"
    path = "SocialNetworkCLI/bin/%s/SocialNetworkCLI.exe" % build_configuration
    launchers = {"Windows": (path, []), "Linux": ("mono", path)}
    (launcher, arguments) = launchers[platform.system()]
    context.process = subprocess.Popen(args = [launcher, arguments], stdin=subprocess.PIPE, stdout=subprocess.PIPE)
    context.stdout_queue = queue.Queue()
    context.check_queue = queue.Queue()
    context.stdout_reading_thread = threading.Thread(name = 'stdout reader', target = blocking_read, args=[context])
    context.stdout_reading_thread.start()

def after_scenario(context, scenario):
    if get_returncode(context) == None:
        exit_text = "quit%s" % os.linesep
        (output,_) = context.process.communicate(bytearray(exit_text, 'utf-8'), timeout = 5)
        context.stdout_queue.put_nowait(output)
    if get_returncode(context) == None:
        context.process.kill()
    whole_output = ""
    while not context.stdout_queue.empty():
        output_piece = context.stdout_queue.get()
        whole_output += str(output_piece)
    while not context.check_queue.empty():
        text_to_check = context.check_queue.get()
        assert text_to_check in whole_output
