import threading
import time
shared_counter = 0
mutex = threading.Lock()

def increment_counter():
    global shared_counter
    mutex.acquire()
    try:
        shared_counter += 1
        print(f"Thread {threading.current_thread().name} incremented counter to {shared_counter}")
    finally:
        mutex.release()

threads = []
for i in range(3):
    thread = threading.Thread(target=increment_counter, name=f"Thread-{i}")
    threads.append(thread)
for thread in threads:
    thread.start()
for thread in threads:
    thread.join()
print("Final value of shared counter:", shared_counter)
