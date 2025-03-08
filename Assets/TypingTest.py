import time
import random
import os

# prompt_toolkit 임포트
from prompt_toolkit import PromptSession

# PromptSession 객체 생성 (전역으로 한 번만 생성)
session = PromptSession()

def input_with_prefill(prompt, text):
    """
    prompt_toolkit의 PromptSession을 이용해
    이전 입력값(text)을 'default'로 설정하여
    편집 가능한 상태로 입력받습니다.
    """
    return session.prompt(prompt, default=text)

def print_random_sentence(length):
    """
    phrases_19_20_21_for_real_test.txt 파일에서 문장들을 읽어,
    인자로 전달된 길이와 같은 문장들 중 하나를 무작위로 선택하여 출력하고 반환합니다.
    """
    try:
        with open("phrases_19_20_21_for_real_test.txt", "r", encoding="utf-8") as file:
            sentences = file.read().splitlines()
    except FileNotFoundError:
        print("phrases_19_20_21_for_real_test.txt 파일을 찾을 수 없습니다.")
        return None

    # 주어진 길이와 정확히 일치하는 문장들 필터링
    valid_sentences = [sentence for sentence in sentences if len(sentence) == length]
    if not valid_sentences:
        print(f"길이가 {length}인 문장이 없습니다.")
        return None

    sentence = random.choice(valid_sentences)
    return sentence

def main():
    # 플레이어 번호 입력 (숫자가 아니면 계속 재입력)
    while True:
        player_number = session.prompt("플레이어 번호를 입력하면 시작됩니다. \n플레이어 번호: ")
        if player_number.isdigit():
            break
        else:
            print("숫자를 입력해주세요.")

    # 3초간 카운트다운
    print("카운트다운 시작!")
    for i in range(3, 0, -1):
        print(i)
        time.sleep(1)

    total_time = 0.0

    # 현재 작업 디렉토리에 results 폴더 생성 (없으면 생성)
    current_dir = os.getcwd()
    results_folder = os.path.join(current_dir, "results")
    if not os.path.exists(results_folder):
        os.makedirs(results_folder)

    # 결과 저장 파일 경로 설정
    result_file_path = os.path.join(results_folder, f"Player_{player_number}_Keyboard_real.txt")

    # 3회 반복: 각 회마다 문장 길이가 19, 20, 21
    for i in range(3):
        sentence_length = 19 + i
        sentence = print_random_sentence(sentence_length)
        if sentence is None:
            continue  # 해당 길이의 문장이 없으면 다음 회차로 넘어감

        print("문장을 그대로 입력하세요")
        print(sentence)

        # 타이핑 시간 측정 시작
        start_time = time.time()

        # 사용자가 입력한 마지막 내용을 저장할 변수 (처음엔 빈 문자열)
        user_input_text = ""

        while True:
            # 재입력 시, 바로 직전 입력값을 미리 채워놓기
            user_input_text = input_with_prefill("", user_input_text)

            if user_input_text == sentence:
                end_time = time.time()
                elapsed = end_time - start_time
                total_time += elapsed
                # 결과 파일에 기록 (파일이 존재하면 개행 후 추가)
                with open(result_file_path, "a", encoding="utf-8") as f:
                    f.write(sentence + "\n")
                    f.write(f"{elapsed:.3f}\n")
                break
            else:
                print("입력이 문장과 다릅니다. 다시 입력해주세요.")
                print(sentence)

        print("\n")

    # 모든 과정 종료 후 Total Time 기록
    with open(result_file_path, "a", encoding="utf-8") as f:
        f.write(f"Total Time: {total_time:.3f}\n")
    print("\n모든 타이핑 테스트가 완료되었습니다.")

if __name__ == "__main__":
    main()
