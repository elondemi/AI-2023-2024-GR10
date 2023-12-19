import sys
from PyQt5.QtWidgets import (
    QApplication, QWidget, QPushButton, QComboBox, QLabel, QFileDialog,
    QMessageBox, QVBoxLayout, QHBoxLayout, QSizePolicy
)
from dfs_sudoku_solver import solve_sudoku_dfs
from bfs_sudoku_solver import solve_sudoku_bfs 

import pandas as pd
import numpy as np

def check_table(path):
    try:
        df = pd.read_excel(path)
        if df.shape != (9, 9):
            return {"response": "TABLE SHOULD BE IN 9x9 FORMAT"}
        df.fillna(0, inplace=True)
        if not df.apply(lambda col: col.map(lambda x: x in range(10))).all().all():
            return {"response": "ALL VALUES SHOULD BE INTEGERS BETWEEN 0 AND 9"}
        return {"response": "200"}

    except Exception as e:
        return {"response": f"Error reading file: {e}"}


class SolverGUI(QWidget):
    def __init__(self):
        super().__init__()
        self.file_path_sudoku = ""
        self.file_path_social_golfers = ""
        self.main_layout = QVBoxLayout()
        self.initUI()

    def initUI(self):
        self.setWindowTitle('Problem Solver GUI')
        self.setGeometry(100, 100, 500, 300)

        self.lbl_problem = QLabel('Select Problem:', self)
        self.cb_problem = QComboBox(self)
        self.cb_problem.addItems(['Sudoku', 'Social Golfers'])
        self.cb_problem.currentIndexChanged.connect(self.update_problem_selection)

        self.lbl_import_sudoku = QLabel('Import Sudoku file:', self)
        self.btn_import_sudoku = QPushButton('Browse Sudoku', self)
        self.btn_import_sudoku.clicked.connect(lambda: self.import_file('sudoku'))

        self.lbl_import_social_golfers = QLabel('Import Social Golfers file:', self)
        self.btn_import_social_golfers = QPushButton('Browse Social Golfers', self)
        self.btn_import_social_golfers.clicked.connect(lambda: self.import_file('social_golfers'))

        self.lbl_algorithm = QLabel('Select Algorithm:', self)
        self.cb_algorithm = QComboBox(self)
        self.cb_algorithm.addItems(['DFS Backtracking', 'BFS Backtracking'])

        self.btn_solve = QPushButton('Solve', self)
        self.btn_solve.clicked.connect(self.solve_problem)

        problem_layout = QHBoxLayout()
        problem_layout.addWidget(self.lbl_problem)
        problem_layout.addWidget(self.cb_problem)

        file_layout_sudoku = QHBoxLayout()
        file_layout_sudoku.addWidget(self.lbl_import_sudoku)
        file_layout_sudoku.addWidget(self.btn_import_sudoku)

        file_layout_social_golfers = QHBoxLayout()
        file_layout_social_golfers.addWidget(self.lbl_import_social_golfers)
        file_layout_social_golfers.addWidget(self.btn_import_social_golfers)

        algorithm_layout = QHBoxLayout()
        algorithm_layout.addWidget(self.lbl_algorithm)
        algorithm_layout.addWidget(self.cb_algorithm)

        solve_layout = QHBoxLayout()
        solve_layout.addWidget(self.btn_solve)

        self.main_layout.addLayout(problem_layout)
        self.main_layout.addLayout(file_layout_sudoku)
        self.main_layout.addLayout(file_layout_social_golfers)
        self.main_layout.addLayout(algorithm_layout)
        self.main_layout.addLayout(solve_layout)

        spacer = QLabel('', self)
        spacer.setSizePolicy(QSizePolicy.Expanding, QSizePolicy.Expanding)
        self.main_layout.addWidget(spacer)

        self.setLayout(self.main_layout)
        self.update_problem_selection()  

    def update_problem_selection(self):
        selected_problem = self.cb_problem.currentText()
        if selected_problem == 'Sudoku':
            self.lbl_import_sudoku.show()
            self.btn_import_sudoku.show()
            self.lbl_import_social_golfers.hide()
            self.btn_import_social_golfers.hide()
        elif selected_problem == 'Social Golfers':
            self.lbl_import_sudoku.hide()
            self.btn_import_sudoku.hide()
            self.lbl_import_social_golfers.show()
            self.btn_import_social_golfers.show()

    def import_file(self, problem_type):
        file_path, _ = QFileDialog.getOpenFileName(self, 'Import File', '', 'Excel Files (*.xlsx)')
        if file_path:
            response = check_table(file_path)  
            if response and response["response"] == "200":
                QMessageBox.information(self, 'Success', f'File {file_path} imported successfully!')
                if problem_type == 'sudoku':
                    self.file_path_sudoku = file_path
                elif problem_type == 'social_golfers':
                    self.file_path_social_golfers = file_path
            else:
                QMessageBox.warning(self, 'Error', f'Error in file import. Code: {response["response"] if response else "Unknown error"}')

    def solve_problem(self):
        selected_problem = self.cb_problem.currentText()
        algorithm = self.cb_algorithm.currentText()
        file_path = self.file_path_sudoku if selected_problem == 'Sudoku' else self.file_path_social_golfers

        if file_path == "":
            QMessageBox.warning(self, 'Error', 'No file imported. Please import a file.')
            return

        if selected_problem == 'Sudoku':
            if algorithm == 'DFS Backtracking':
                solution, time_taken = solve_sudoku_dfs(file_path)
            elif algorithm == 'BFS Backtracking':
                solution, time_taken = solve_sudoku_bfs(file_path)
            else:
                QMessageBox.warning(self, 'Error', 'Algorithm not supported.')
                return

            if solution is not None:
                QMessageBox.information(self, 'Success', f'Sudoku solved in {time_taken:.2f} seconds!')
                print("Sudoku Solution:\n", solution)
            else:
                QMessageBox.warning(self, 'Error', 'Sudoku puzzle could not be solved.')

if __name__ == '__main__':
    app = QApplication(sys.argv)
    solver_gui = SolverGUI()
    solver_gui.show()
    sys.exit(app.exec_())
