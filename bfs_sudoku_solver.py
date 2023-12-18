import pandas as pd
import numpy as np
from queue import Queue
import time

def is_valid(board, row, col, num):
    for i in range(9):
        if board[row][i] == num or board[i][col] == num:
            return False
    start_row, start_col = 3 * (row // 3), 3 * (col // 3)
    for i in range(3):
        for j in range(3):
            if board[start_row + i][start_col + j] == num:
                return False
    return True

def find_next_empty(board):
    for i in range(9):
        for j in range(9):
            if board[i][j] == 0:
                return i, j
    return None, None

def solve_sudoku_bfs(path):
    df = pd.read_excel(path)
    board = df.fillna(0).to_numpy().astype(int)
    q = Queue()
    q.put(board)

    start_time = time.time()

    while not q.empty():
        current_board = q.get()
        row, col = find_next_empty(current_board)

        if row is None:
            end_time = time.time()
            return current_board, end_time - start_time

        for num in range(1, 10):
            if is_valid(current_board, row, col, num):
                new_board = np.copy(current_board)
                new_board[row][col] = num
                q.put(new_board)

    end_time = time.time()
    return None, end_time - start_time
