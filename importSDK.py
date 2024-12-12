import subprocess
import shutil
import os
import logging

logging.basicConfig(level=logging.INFO, format='%(asctime)s - %(levelname)s - %(message)s')

def update_submodules():
    try:
        logging.info("Updating Submodule...")
        subprocess.check_call(["git", "submodule", "update", "--init", "--recursive"])
        logging.info("Updating Submodule Completed.")
    except subprocess.CalledProcessError as e:
        logging.error(f"Updating Submodule Failed: {e}")
        exit(1)

def copy_sdk_assets(sdk_path="SDK", dest_path="Assets"):

    source_path = os.path.join(sdk_path, "Assets")
    if not os.path.exists(source_path):
        logging.error(f"Cannot Find: {source_path}")
        exit(1)

    if not os.path.exists(dest_path):
        os.makedirs(dest_path)
        logging.info(f"Create: {dest_path}")

    try:
        logging.info(f"Copy {source_path} to {dest_path}...")
        shutil.copytree(source_path, dest_path, dirs_exist_ok=True, ignore_dangling_symlinks=True)
        logging.info("Copy Completed.")
    except shutil.Error as e:
        logging.error(f"Copy Failed: {e}")
        exit(1)
    except OSError as e:
        logging.error(f"Copy Failed Authoration: {e}")
        exit(1)


if __name__ == "__main__":
    update_submodules()
    copy_sdk_assets()
