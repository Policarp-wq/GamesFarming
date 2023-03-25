using System;

namespace GamesFarming.MVVM.Models.Steam
{
    public class FarmProgress
    {
        private int _accountsCnt;

        public int AccountsCnt
        {
            get { return _accountsCnt; }
            set 
            {
                _accountsCnt = value;
                Update();
            }
        }
        public int Percents => AccountsCnt > 0 ? Convert.ToInt32(Math.Round(100.0 * Current / AccountsCnt)) : 100;

        public int Current { get; private set; }
        public int Step { get; set; }
        public event Action Updated;
        public FarmProgress()
        {
            AccountsCnt = 0;
            Current = 0;
            Step = 0;
        }
        public void Reset()
        {
            AccountsCnt = 0;
            Current = 0;
            Step = 0;
            Updated?.Invoke();
        }
        public void Up()
        {
            Current += Step;
            if(Current > AccountsCnt)
                Current = AccountsCnt;
            Update();
        }
        private void Update()
        {
            Updated?.Invoke();
        }

    }
}
