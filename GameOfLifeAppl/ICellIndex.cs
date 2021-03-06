﻿namespace GameOfLifeAppl
{
    internal interface ICellIndex
    {
        int Col { get; }

        int Row { get; }

        bool IsLifeCell { get; }

        bool IsEmptyCell { get; }
 
        bool IsDyingCell { get; }
        
        bool IsNewCell { get; }

        void SetDyingCell();

        void SetNewCell();

        void SetEmptyCell();
        
        void SetLifeCell();

        void IncGeneration();

        int Generation { get; }
    }
}