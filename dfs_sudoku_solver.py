import pandas as pd
import numpy as np
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

def find_empty_cell(board):
    for i in range(9):
        for j in range(9):
            if board[i][j] == 0:
                return i, j
    return None

def dfs_solve_sudoku(board):
    find = find_empty_cell(board)
    if not find:
        return True
    row, col = find
    for num in range(1, 10):
        if is_valid(board, row, col, num):
            board[row][col] = num
            if dfs_solve_sudoku(board):
                return True
            board[row][col] = 0
    return False

def solve_sudoku_dfs(path):
    df = pd.read_excel(path)
    matrix = df.fillna(0).to_numpy().astype(int)

    start_time = time.time()
    if dfs_solve_sudoku(matrix):
        end_time = time.time()
        return matrix, end_time - start_time
    else:
        return None, 0

